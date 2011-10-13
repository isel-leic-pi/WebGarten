namespace PI.WebGarten.Demos.Todos
{
    using System.Diagnostics;

    using PI.WebGarten.Demos.Todos.Controllers;
    using PI.WebGarten.Demos.Todos.Model;
    using PI.WebGarten.MethodBasedCommands;

    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
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
