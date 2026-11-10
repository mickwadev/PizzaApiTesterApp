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
	TokenStorage tokenStorage,
	IHttpClientFactory httpClientFactory) : ObservableObject
{
	[ObservableProperty]
	private string _info;

	[ObservableProperty]
	private string _userName;

	[ObservableProperty]
	private string _password;
	[RelayCommand]
	private async Task GetIngredients()
	{
		try
		{
			ICollection<IngredientDto> ingregients = await pizzaApi.Ingredients();
			Info = ingregients.First().Name;
		}
		catch (Exception ex) 
		{ 
			Info = ex.Message;
		}
	}

	[RelayCommand]
	private async Task CreateUser()
	{
		try
		{
			var result = await pizzaApi.Create(new CreateUserRequestDto() { Name = UserName, Password = Password });
			Info = result;
		}
		catch (Exception ex) 
		{
			Info = ex.Message;
		}
	}

	[RelayCommand]
	private async Task LoginUser()
	{
		var logged = await pizzaApi.Login(new LoginUserRequestDto { Name=UserName, Password=Password});
		await tokenStorage.StoreTokens(new(logged.Jwt,logged.RefreshToken));
	}

	[RelayCommand]
	private async Task FactoryTest()
	{
		var client = httpClientFactory.CreateClient("PanMors");
		var response = await client.GetAsync("/pizza/panmors");
		var content = await response.Content.ReadAsStringAsync();
		HttpRequestMessage request = response.RequestMessage;
		
		Info = $"{content} {request.RequestUri}";
	}

}