using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using PI.WebGarten.Html;
using PI.WebGarten.MethodBasedCommands;

namespace PI.WebGarten.Demos.Todos
{
    class ToDosController
    {
        private readonly IToDoRepository _repo;
        public ToDosController()
        {
            _repo = ToDoRepositoryLocator.Get();
        }
        
        [HttpCmd(HttpMethod.Get, "/todos")]
        public HttpResponse Get()
        {
            return new HttpResponse(200, new TodosView(_repo.GetAll()));
        }

        [HttpCmd(HttpMethod.Post, "/todos")]
        public HttpResponse Post(IEnumerable<KeyValuePair<string,string>> content)
        {
            var desc = content.Where(p => p.Key == "desc").Select(p => p.Value).FirstOrDefault();
            if (desc == null)
            {
                return new HttpResponse(HttpStatusCode.BadRequest);
            }
            var td = new ToDo {Description = desc};
            _repo.Add(td);
            return new HttpResponse(HttpStatusCode.SeeOther).WithHeader("Location",ResolveUri.For(td));
        }

    }

    class TodosView : HtmlDoc
    {
        public TodosView(IEnumerable<ToDo> t)
            :base("To Dos",
                H1(Text("To Do list")),
                Ul(
                    t.Select(td => Li(A(ResolveUri.For(td),td.Description))).ToArray()
                ),
                H2(Text("Create a new ToDo")),
                Form("post","/todos",
                    Label("desc","Description: "),InputText("desc")
                )
            ){}
    }

    class ToDoController
    {
        private readonly IToDoRepository _repo;
        public ToDoController()
        {
            _repo = ToDoRepositoryLocator.Get();
        }

        [HttpCmd(HttpMethod.Get, "/todos/{id}")]
        public HttpResponse Get(int id)
        {
            var td = _repo.GetById(id);
            return td == null ? new HttpResponse(HttpStatusCode.NotFound) : new HttpResponse(200, new TodoView(td));
        }
    }

    class TodoView : HtmlDoc
    {
        public TodoView(ToDo t)
            :base("To Dos",
                H1(Text("To Do")),
                P(Text(t.Description)),
                A(ResolveUri.ForTodos(),"ToDo list")
            ){}
    }

    static class ResolveUri
    {
        public static string For(ToDo td)
        {
            return string.Format("/todos/{0}", td.Id);
        }

        public static string ForTodos()
        {
            return "/todos";
        }
    }

    class ToDo
    {
        public int Id { get; set; }
        public string Description { get; set; }    
    }

    interface IToDoRepository  
    {
        IEnumerable<ToDo> GetAll();
        ToDo GetById(int id);
        void Add(ToDo td);
    }

    class ToDoMemoryRepository : IToDoRepository
    {
        private readonly IDictionary<int, ToDo> _repo = new Dictionary<int, ToDo>();
        private int _cid = 0;

        public IEnumerable<ToDo> GetAll()
        {
            return _repo.Values;
        }

        public ToDo GetById(int id)
        {
            ToDo td = null;
            _repo.TryGetValue(id, out td);
            return td;
        }

        public void Add(ToDo td)
        {
            td.Id = _cid;
            _repo.Add(_cid++,td);
        }
    }

    class ToDoRepositoryLocator
    {
        private readonly static IToDoRepository Repo = new ToDoMemoryRepository();
        public static IToDoRepository Get()
        {
            return Repo;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var repo = ToDoRepositoryLocator.Get();
            repo.Add(new ToDo {Description = "Learn HTTP better"});
            repo.Add(new ToDo { Description = "Learn HTML 5 better"});
            var host = new HttpListenerBasedHost("http://localhost:8080/");
            host.Add(DefaultMethodBasedCommandFactory.GetCommandsFor(
                typeof(ToDosController),
                typeof(ToDoController)));
            host.OpenAndWaitForever();
        }
    }
}
