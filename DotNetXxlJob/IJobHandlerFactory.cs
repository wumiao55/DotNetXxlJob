namespace DotNetXxlJob
{
    public interface IJobHandlerFactory
    {
        IJobHandler GetJobHandler(string handlerName);
    }
}