namespace puzzles.Models
{
    public interface IGenerator<out T>
    {
        T Generate(params object[] options);
    }
}
