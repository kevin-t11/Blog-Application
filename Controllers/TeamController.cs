
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Services.WebApi;

public class TeamController : Controller
{
    public IActionResult Team()
    {
        // You can fetch team members' data from a database or any other source here
        // For simplicity, let's assume you have a list of team members
        var teamMembers = GetTeamMembers();

        return View(teamMembers);
    }

    private List<TeamMember> GetTeamMembers()
    {
        // Fetch team members' data from your data source
        // For now, let's assume you have a hardcoded list of team members
        return new List<TeamMember>
        {
            new TeamMember { Name = "Kevin Thumbar", Role = "Developer", Description = "Experienced developer with a passion for coding." },
            new TeamMember { Name = "Zeel Vaghasiya", Role = "Designer", Description = "Creative designer with an eye for aesthetics." }
            // Add more team members as needed
        };
    }
}
