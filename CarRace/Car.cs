public partial class Program
{
    public class Car
    {
        public int? id { get; set; }
        public string? name { get; set; }

        public decimal speed { get; set; }

        public double time { get; set; }

        public decimal distanceDriven { get; set; }

        public decimal distanceLeft { get; set; }
        // Method that tells how much time is left until a car reaches the goal in hours, must do (result*3600) to get it in seconds
        public decimal timeLeft()
        {

            

            decimal remainingTime = distanceLeft / speed;

            return remainingTime;

        }
        // All my delay methods for the random uhOH moments
        public async Task outOfGas()
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
        }
        public async Task flatTire()
        {
            await Task.Delay(TimeSpan.FromSeconds(20));
        }
        public async Task birdOnWindshield()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            
        }
       


    }
}


