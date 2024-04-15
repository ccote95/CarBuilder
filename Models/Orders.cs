namespace CarBuilder.Models;
public class Orders
{
    public int Id { get; set; }
    public int WheelId { get; set; }
    public int TechnologyId { get; set; }
    public int PaintId { get; set; }
    public int InteriorId { get; set; }
    public DateTime Timestamp { get; set; }
    public PaintColor PaintColor { get; set; }
    public Interior Interior { get; set; }
    public Technology Technology { get; set; }
    public Wheels Wheels { get; set; }
    public bool IsComplete { get; set; }

}