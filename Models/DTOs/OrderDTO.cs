namespace CarBuilder.Models.DTOs;
public class OrdersDTO
{
    public int Id { get; set; }
    public int WheelId { get; set; }
    public int TechnologyId { get; set; }
    public int PaintId { get; set; }
    public int InteriorId { get; set; }
    public PaintColorDTO PaintColor { get; set; }
    public InteriorDTO Interior { get; set; }
    public WheelsDTO Wheels { get; set; }
    public TechnologyDTO Technology { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsComplete { get; set; }
    public decimal TotalCost
    {
        get
        {
            decimal total = 0;
            total += Wheels.Price + Technology.Price + PaintColor.Price + Interior.Price;
            return total;

        }
    }


}