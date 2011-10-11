namespace PI.WebGarten.Demos.Todos
{
    using PI.WebGarten.Demos.Todos.Model;

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
}