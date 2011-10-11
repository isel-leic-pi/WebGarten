using System;

namespace PI.WebGarten
{
    public interface ICommand {
        UriTemplate UriTemplate {get;}
        string HttpMethod{get;}

        HttpResponse Execute(RequestInfo req);	
    }
}