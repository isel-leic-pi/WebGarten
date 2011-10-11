namespace PI.WebGarten.Demos.Todos
{
    using PI.WebGarten.Demos.Todos.Model;
    using PI.WebGarten.Html;

    class TodoView : HtmlDoc
    {
        public TodoView(ToDo t)
            :base("To Dos",
                H1(Text("To Do")),
                P(Text(t.Description)),
                A(ResolveUri.ForTodos(),"ToDo list")
                ){}
    }
}