using ApiService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PizzaApiTesterApp;

public partial class AuthTestsPage : ContentPage
{
	public AuthTestsPage(AuthTestsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}

public partial class AuthTestsPageViewModel(IPizzaApiv1 pizzaApi,
	TokenStorage tokenStorage) : ObservableObject
{
	[RelayCommand]
	private async Task GetIngredients()
	{
		var ingregients = await pizzaApi.Ingredients();
	}
	[RelayCommand]
	private async Task LoginUser()
	{
		var logged = await pizzaApi.Login(new LoginUserRequestDto { Name="Kulfon", Password="haslo"});
		await tokenStorage.StoreTokens(new(logged.Jwt,logged.RefreshToken));
	}

}