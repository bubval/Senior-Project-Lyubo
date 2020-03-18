namespace Phone_Forecast.Models
{
    public class Testingasd
    {
        private readonly int value;

        public Testingasd(int value)
        {
            this.value = value;
        }

        public Testingasd Where(int x)
        {
            return new Testingasd(this.value + x);
        }

        public Testingasd Where2(int x)
        {
            return new Testingasd(this.value - x);
        }
    }
}
