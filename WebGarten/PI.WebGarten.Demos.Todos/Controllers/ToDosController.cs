namespace PI.WebGarten.Demos.Todos.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using PI.WebGarten.Demos.Todos.Model;
    using PI.WebGarten.MethodBasedCommands;

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
        public HttpResponse Post(IEnumerable<KeyValuePair<string, string>> content)
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
}