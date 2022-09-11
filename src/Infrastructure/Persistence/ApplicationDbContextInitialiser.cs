using OMS.Domain.Entities;
using OMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Object = OMS.Domain.Entities.Object;

namespace OMS.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "admin@localhost", Email = "admin@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Admin1!");
            await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }

        // Default data
        // Seed, if necessary

        if (!_context.ObjectTypes.Any())
        {
            _context.ObjectTypes.Add(new ObjectType
            {
                Name = "Fruit",
                Created = DateTime.UtcNow,
                CreatedBy = "test user",
                LastModified = DateTime.UtcNow,
                LastModifiedBy = "test user"
            });

            _context.ObjectTypes.Add(new ObjectType
            {
                Name = "Vegetable",
                Created = DateTime.UtcNow,
                CreatedBy = "test user",
                LastModified = DateTime.UtcNow,
                LastModifiedBy = "test user"
            });

            _context.ObjectTypes.Add(new ObjectType
            {
                Name = "Food",
                Created = DateTime.UtcNow,
                CreatedBy = "test user",
                LastModified = DateTime.UtcNow,
                LastModifiedBy = "test user"
            });

            await _context.SaveChangesAsync();
        }

        if (!_context.Objects.Any())
        {
            var fruitObjectType = await _context.ObjectTypes.FirstAsync(o => o.Name == "Fruit");
            var vegetableObjectType = await _context.ObjectTypes.FirstAsync(o => o.Name == "Vegetable");

            _context.Objects.Add(new Object
            {
                Name = "Mango",
                Description = "A mango is a sweet tropical fruit, and it's also the name of the trees on which the fruit grows",
                ObjectType = fruitObjectType                
            });

            _context.Objects.Add(new Object
            {
                Name = "Guava",
                Description = "The fruits are round to pear-shaped and measure up to 7.6 cm in diameter; their pulp contains many small hard seeds (more abundant in wild forms than in cultivated varieties). The fruit has a yellow skin and white, yellow, or pink flesh. The musky, at times pungent, odour of the sweet pulp is not always appreciated.",
                ObjectType = fruitObjectType
            });

            var lettuce = new Object
            {
                Name = "Lettuce",
                Description = "Lettuce (Lactuca sativa) is an annual plant of the family Asteraceae. It is most often grown as a leaf vegetable, but sometimes for its stem and seeds. Lettuce is most often used for salads, although it is also seen in other kinds of food, such as soups, sandwiches and wraps; it can also be grilled.",
                ObjectType = vegetableObjectType
            };

            var tomato = new Object
            {
                Name = "Tomato",
                Description = "They are usually red, scarlet, or yellow, though green and purple varieties do exist, and they vary in shape from almost spherical to oval and elongate to pear-shaped.",
                ObjectType = vegetableObjectType
            };

            var onion = new Object
            {
                Name = "Onion",
                Description = "An onion is a round vegetable with a brown skin that grows underground. It has many white layers on its inside which have a strong, sharp smell and taste.",
                ObjectType = vegetableObjectType
            };

            var cucumber = new Object
            {
                Name = "Cucumber",
                Description = "Cucumber is a summer vegetable, with elongate shape and 15cm long. Its skin is of a green colour, turning into yellow in maturation.",
                ObjectType = vegetableObjectType
            };

            var salad = new Object
            {
                Name = "Salad",
                Description = "a mixture of raw usually green leafy vegetables (as lettuce) combined with other vegetables (as tomato and cucumber) and served with a dressing.",
                ObjectType = vegetableObjectType                
            };

            _context.Objects.Add(tomato);
            _context.Objects.Add(cucumber);
            _context.Objects.Add(onion);
            _context.Objects.Add(salad);
            _context.ObjectRelationships.Add(new ObjectRelationship { Object = salad, RelatedObject = lettuce });
            _context.ObjectRelationships.Add(new ObjectRelationship { Object = salad, RelatedObject = tomato });
            _context.ObjectRelationships.Add(new ObjectRelationship { Object = salad, RelatedObject = cucumber });
            _context.ObjectRelationships.Add(new ObjectRelationship { Object = salad, RelatedObject = onion });

            await _context.SaveChangesAsync();
        }


    }
}
