using Tests.Akeneo.Base;
using Apps.Akeneo.Actions;

namespace Tests.Akeneo;

[TestClass]
public class GeneralActionsTests : TestBase
{
	[TestMethod]
	public async Task GetAllLocales_ReturnsLocales()
	{
		// Arrange
		var actions = new GeneralActions(InvocationContext);

		// Act
		var result = await actions.GetAllLocales();

		// Assert
		foreach (var locale in result.Locales)
		{
            Console.WriteLine(locale);
		}
		Assert.IsNotNull(result);
	}
}
