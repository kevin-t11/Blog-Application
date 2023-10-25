using Microsoft.AspNetCore.Mvc;

public class ContactController : Controller
{
    public IActionResult Contact()
    {
        return View(new ContactViewModel());
    }

    [HttpPost]
    public IActionResult Contact(ContactViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Process the form data, e.g., send email, save to database, etc.
            // Redirect to a success page or show a thank you message.
            // For now, let's just redirect back to the Contact page.
            return RedirectToAction(nameof(Contact));
        }

        // If ModelState is not valid, return the view with validation errors.
        return View(model);
    }
}
