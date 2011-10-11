namespace PI.WebGarten.Demos.Todos.Controllers
{
    using System.Net;

    using PI.WebGarten.Demos.Todos.Model;
    using PI.WebGarten.MethodBasedCommands;

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
}