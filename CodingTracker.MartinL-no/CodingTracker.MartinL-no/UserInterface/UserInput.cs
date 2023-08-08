using CodingTracker.MartinL_no.Controllers;

namespace CodingTracker.MartinL_no.UserInterface;

internal class UserInput
{
    private readonly CodingController _controller;

    public UserInput(CodingController controller)
	{
        _controller = controller;
    }
}
