using CarBuilder.Models;
using CarBuilder.Models.DTOs;
List<PaintColor> paintColors = new List<PaintColor>{
    new()
    {
        Id = 1,
        Price = 100.00M,
        Color = "Silver"
    },
    new()
    {
        Id = 2,
        Price = 150.00M,
        Color = "Midnight Blue"
    },
    new()
    {
        Id = 3,
        Price = 200.00M,
        Color = "Firebrick Red"
    },
    new()
    {
        Id = 4,
        Price = 250.00M,
        Color = "Spring Green"
    },
};
List<Interior> interiors = new List<Interior>{
    new()
    {
        Id = 1,
        Price = 90.00M,
        Material = "Beige Fabric"
    },
    new()
    {
        Id = 2,
        Price = 100.00M,
        Material = "Charcoal Fabric"
    },
    new()
    {
        Id = 3,
        Price = 150.00M,
        Material = "White Leather"
    },
    new()
    {
        Id = 4,
        Price = 200.00M,
        Material = "Black Leather"
    },
};
List<Technology> technologies = new List<Technology>{
            new()
            {
                Id = 1,
                Price = 50M,
                Package = "Basic Package (basic sound system)"
            },
            new()
            {
                Id = 2,
                Price = 100M,
                Package = "Navigation Package (includes integrated navigation controls)"
            },
            new()
            {
                Id = 3,
                Price = 150M,
                Package = "Visibility Package (includes side and rear cameras)"
            },
            new()
            {
                Id = 4,
                Price = 200M,
                Package = "Ultra Package (includes navigation and visibility packages)"
            },
    };
List<Wheels> wheels = new List<Wheels>{
    new()
    {
        Id=1,
        Price = 50M,
        Style = "17-inch Pair Radial"

    },
    new()
    {
        Id=2,
        Price = 75M,
        Style = "17-inch Pair Radial Black"

    },
       new()
    {
        Id=3,
        Price = 100M,
        Style = "18-inch Pair Spoke Silver"

    },
       new()
    {
        Id=4,
        Price = 150M,
        Style = "18-inch Pair Spoke Black"

    },

};
List<Orders> orders = new List<Orders> {
    new()
    {
        Id=1,
        WheelId = 2,
        TechnologyId = 1,
        PaintId = 3,
        InteriorId = 1,
        Timestamp = new DateTime(2024, 04, 12),
        IsComplete = false


    },
        new()
    {
        Id=2,
        WheelId = 1,
        TechnologyId = 2,
        PaintId = 2,
        InteriorId = 3,
        Timestamp = new DateTime(2024, 04, 15),
        IsComplete = false


    },
        new()
    {
        Id=3,
        WheelId = 1,
        TechnologyId = 4,
        PaintId = 4,
        InteriorId = 4,
        Timestamp = new DateTime(2024, 04, 02),
        IsComplete = false


    },
 };




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
                {
                    options.AllowAnyOrigin();
                    options.AllowAnyMethod();
                    options.AllowAnyHeader();
                });
}

app.UseHttpsRedirection();

// for getting all the wheel options
app.MapGet("/Wheels", () =>
{
    return wheels.Select(w => new WheelsDTO
    {
        Id = w.Id,
        Price = w.Price,
        Style = w.Style
    });
});

// for getting all technology options
app.MapGet("/Technology", () =>
{
    return technologies.Select(t => new TechnologyDTO
    {
        Id = t.Id,
        Price = t.Price,
        Package = t.Package
    });
});

// for getting all interior options
app.MapGet("/Interior", () =>
{
    return interiors.Select(i => new InteriorDTO
    {
        Id = i.Id,
        Price = i.Price,
        Material = i.Material
    });
});

// for getting all paint color options
app.MapGet("/PaintColor", () =>
{
    return paintColors.Select(p => new PaintColorDTO
    {
        Id = p.Id,
        Price = p.Price,
        Color = p.Color
    });
});

// for getting all orders
app.MapGet("/orders", (int? paintId) =>
{
    foreach (Orders order in orders)
    {
        order.Wheels = wheels.First(w => w.Id == order.WheelId);
        order.Technology = technologies.First(w => w.Id == order.TechnologyId);
        order.PaintColor = paintColors.First(w => w.Id == order.PaintId);
        order.Interior = interiors.First(w => w.Id == order.InteriorId);
    }

    List<Orders> filteredOrders = orders.Where(o => !o.IsComplete).ToList();

    // Now, check for the paintId property to see if we should filter by that as well
    if (paintId != null)
    {
        filteredOrders = filteredOrders.Where(order => order.PaintId == paintId).ToList();
    }

    return filteredOrders.Select(o => new OrdersDTO
    {
        Id = o.Id,
        Timestamp = o.Timestamp,
        TechnologyId = o.TechnologyId,
        Technology = new TechnologyDTO
        {
            Id = o.Technology.Id,
            Package = o.Technology.Package,
            Price = o.Technology.Price
        },
        WheelId = o.WheelId,
        Wheels = new WheelsDTO
        {
            Id = o.Wheels.Id,
            Style = o.Wheels.Style,
            Price = o.Wheels.Price
        },
        InteriorId = o.InteriorId,
        Interior = new InteriorDTO
        {
            Id = o.Interior.Id,
            Material = o.Interior.Material,
            Price = o.Interior.Price
        },
        PaintId = o.PaintId,
        PaintColor = new PaintColorDTO
        {
            Id = o.PaintColor.Id,
            Color = o.PaintColor.Color,
            Price = o.PaintColor.Price
        },
    }).ToList();
});


//creates a new order
app.MapPost("/orders/create", (Orders order) =>
{
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    Technology technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    Interior interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
    PaintColor paintColor = paintColors.FirstOrDefault(p => p.Id == order.PaintId);
    // adds a new Id to the new order
    order.Id = orders.Max(o => o.Id) + 1;
    // adds a new timestamp to the order
    order.Timestamp = DateTime.Now;
    orders.Add(order);

    return Results.Created($"orders/{order.Id}", new OrdersDTO
    {
        Id = order.Id,
        WheelId = order.WheelId,
        Wheels = new WheelsDTO
        {
            Id = wheel.Id,
            Price = wheel.Price,
            Style = wheel.Style
        },
        TechnologyId = order.TechnologyId,
        Technology = new TechnologyDTO
        {
            Id = technology.Id,
            Price = technology.Price,
            Package = technology.Package
        },
        PaintId = order.PaintId,
        PaintColor = new PaintColorDTO
        {
            Id = paintColor.Id,
            Price = paintColor.Price,
            Color = paintColor.Color
        },
        InteriorId = order.InteriorId,
        Interior = new InteriorDTO
        {
            Id = interior.Id,
            Price = interior.Price,
            Material = interior.Material
        },
        Timestamp = DateTime.Now
    });
});

app.MapPost("/orders/{orderId}/fulfill", (int orderId) =>
{
    Orders orderToUpdate = orders.FirstOrDefault(o => o.Id == orderId);
    orderToUpdate.IsComplete = true;
    return Results.Ok();

});

app.Run();


