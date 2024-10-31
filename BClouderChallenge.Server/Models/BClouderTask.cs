public class BClouderTask
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public enum TaskStatus
    {
        Pendente,     // 0
        EmProgresso,  // 1
        Concluido     // 2
    }
}
