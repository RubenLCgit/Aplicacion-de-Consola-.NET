using PetPalApp.Business;

namespace PetPalApp.Presentation;

public class PersonalMenu
{
  public readonly IUserService userService;
  public readonly ISupplierService supplierService;
  public readonly IProductService productService;

  public PersonalMenu(IUserService _userService, ISupplierService _supplierService, IProductService _productService) {
    userService = _userService;
    supplierService = _supplierService;
    productService = _productService;
  }

    public void DisplayPersonalMenu(string name)
  {
    Console.Clear();
    Console.WriteLine(@"
    1. Account information

    2. Offering service

    3. Sell product

    4. My products and services

    5. Remove product or service

    6. Delete account

    7. Back
    ");
    Console.Write("\n\nPlease select an option:");
    SelectServiceOption(name, Console.ReadLine());
  }
  public void SelectServiceOption(string name, string option)
  {
    MainMenu mainMenu = new(userService, supplierService, productService);
    UserMenu userMenu = new(userService, supplierService, productService);
    ServiceMenu serviceMenu = new ServiceMenu(userService, supplierService, productService);
    switch (option)
    {
      case "1":
        Console.WriteLine(userService.ShowAccount(name));
        Console.WriteLine("Press any key to return...");
        Console.ReadLine();
        DisplayPersonalMenu(name);
      break;
      case "2":
        EnterServiceData(name);
      break;
      case "3":
        EnterProductData(name);
      break;
      case "4":
        String listUserServices = supplierService.PrintServices(supplierService.ShowMyServices(name));
        String listUserProducts = productService.PrintProduct(productService.ShowMyProducts(name));
        Console.WriteLine($"{name}'s services:\n\n{listUserServices}\n\n");
        Console.WriteLine($"{name}'s products:\n\n{listUserProducts}\n\n");
        ServiceMenu.PressToContinue();
        DisplayPersonalMenu(name);
      break;
      case "5":
      Console.WriteLine("Delete a service or a product?\n");
      bool optioIncorrect = true;
        String entity = "";
      do
      {
        Console.WriteLine("\n1. Product\n2. Service\n3.Back");
        String entityToDelete = Console.ReadLine();
        switch (entityToDelete)
        {
          case "1":
            entity = "product";
            optioIncorrect = false;
            break;

          case "2":
              entity = "service";
              optioIncorrect = false;
              break;
          case "3":
              DisplayPersonalMenu(name);
              optioIncorrect = false;
              break;
          default:
            Console.WriteLine("\nInvalid option\n");

          break;
        }
      } while (optioIncorrect);
      if (string.IsNullOrEmpty(entity))
      {
        break;
      }
      else
      {
        DeleteUserProductOrService(entity, name);
        MainMenu.EmuleLoad();
        DisplayPersonalMenu(name);
      }
      break;
      case "6":
        DeleteUserProductOrService("account", name);
        MainMenu.EmuleLoad();
        mainMenu.DisplayMainMenu();
      break;
      case "7":
        userMenu.DisplayUserMenu(name);
      break;
      default:
        Console.WriteLine("\nInvalid option\n");
        ServiceMenu.PressToContinue();
        DisplayPersonalMenu(name);
      break;
    }
  }

  private void EnterServiceData(string name) {
    int idUser;
    string type, nameService, description, onlineStr;
    decimal price;
    bool online, isNumber;

    idUser = userService.GetIdUser(name);

    Console.Write("Enter the service name: ");
    nameService = Console.ReadLine();
    while (string.IsNullOrEmpty(nameService))
    {
      Console.Write("\nYou must enter a valid service name: ");
      nameService = Console.ReadLine();
    }
    Console.Write("\nEnter the type of service: ");
    type = Console.ReadLine();
    while (string.IsNullOrEmpty(type))
    {
      Console.Write("\nYou must enter a valid type: ");
      type = Console.ReadLine();
    }
    Console.Write("\nEnter a service description: ");
    description = Console.ReadLine();
    while (string.IsNullOrEmpty(description))
    {
      Console.Write("\nYou must enter a description: ");
      description = Console.ReadLine();
    }
    do
    {
      Console.Write("\nEnter the service price: ");
      string priceStr = Console.ReadLine();
      isNumber = decimal.TryParse(priceStr, out price);
      if (!isNumber)
      {
        Console.Write("\nYou must enter a valid number\n");
      }
      else price = decimal.Parse(priceStr);
    }
    while (!isNumber);

    Console.Write("\nDo you offer shipping services? Y/N: ");
    onlineStr = Console.ReadLine();
    while (string.IsNullOrEmpty(onlineStr) || !string.Equals(onlineStr, "Y", StringComparison.OrdinalIgnoreCase) && !String.Equals(onlineStr, "N", StringComparison.OrdinalIgnoreCase))
    {
      Console.Write("\nYou must enter \"Y\" or \"N\": ");
      onlineStr = Console.ReadLine();
    }
    if (String.Equals(onlineStr, "Y", StringComparison.OrdinalIgnoreCase))
    {
      online = true;
    }
    else
    {
      online = false;
    }
    supplierService.RegisterService(idUser, name, type, nameService, description, price, online);
    
    DisplayPersonalMenu(name);
  }

  private void EnterProductData(string name) {
    int idUser, stock;
    string type, nameProduct, description, onlineStr;
    decimal price;
    bool online, isNumber;

    idUser = userService.GetIdUser(name);

    Console.Write("Enter the product name: ");
    nameProduct = Console.ReadLine();
    while (string.IsNullOrEmpty(nameProduct))
    {
      Console.Write("\nYou must enter a valid product name: ");
      nameProduct = Console.ReadLine();
    }
    Console.Write("\nEnter the type of product: ");
    type = Console.ReadLine();
    while (string.IsNullOrEmpty(type))
    {
      Console.Write("\nYou must enter a valid type: ");
      type = Console.ReadLine();
    }
    Console.Write("\nEnter a product description: ");
    description = Console.ReadLine();
    while (string.IsNullOrEmpty(description))
    {
      Console.Write("\nYou must enter a description: ");
      description = Console.ReadLine();
    }
    do
    {
      Console.Write("\nEnter the product price: ");
      string priceStr = Console.ReadLine();
      isNumber = decimal.TryParse(priceStr, out price);
      if (!isNumber)
      {
        Console.Write("\nYou must enter a valid number\n");
      }
      else price = decimal.Parse(priceStr);
    }
    while (!isNumber);

    do
    {
      Console.Write("\nHow many of these products do you sell?: ");
      string stockStr = Console.ReadLine();
      isNumber = int.TryParse(stockStr, out stock);
      if (!isNumber)
      {
        Console.Write("\nYou must enter a valid number\n");
      }
      else stock = int.Parse(stockStr);
    }
    while (!isNumber);

    Console.Write("\nDo you offer shipping services? Y/N: ");
    onlineStr = Console.ReadLine();
    while (string.IsNullOrEmpty(onlineStr) || !string.Equals(onlineStr, "Y", StringComparison.OrdinalIgnoreCase) && !String.Equals(onlineStr, "N", StringComparison.OrdinalIgnoreCase))
    {
      Console.Write("\nYou must enter \"Y\" or \"N\": ");
      onlineStr = Console.ReadLine();
    }
    if (String.Equals(onlineStr, "Y", StringComparison.OrdinalIgnoreCase))
    {
      online = true;
    }
    else
    {
      online = false;
    }
    productService.RegisterProduct(idUser, name, type, nameProduct, description, price, online, stock);
    
    DisplayPersonalMenu(name);
  }

  private void DeleteUserProductOrService(String entity, String userName)
  {
    Console.WriteLine($"Are you sure you want to delete your {entity}? Y/N: ");
    String response = Console.ReadLine();
    while (string.IsNullOrEmpty(response) || !string.Equals(response, "Y", StringComparison.OrdinalIgnoreCase) && !String.Equals(response, "N", StringComparison.OrdinalIgnoreCase))
    {
      Console.Write("\nYou must enter y or n: ");
      response = Console.ReadLine();
    }
    if (response.Equals("y", StringComparison.CurrentCultureIgnoreCase))
    {
      Console.WriteLine($"Processing deletion of {entity}");
      MainMenu.EmuleLoad();
      if (entity.Equals("account"))
      {
        var userServices = supplierService.ShowMyServices(userName);
        var userProducts = productService.ShowMyProducts(userName);
        foreach (var item in userServices)
        {
          supplierService.DeleteService(userName,item.Value.SupplierId);
        }
        foreach (var item in userProducts)
        {
          productService.DeleteProduct(userName, item.Value.ProductId);
        }
        userService.DeleteUser(userName);
      }
      else if (entity.Equals("service"))
      {
        Console.Write("Enter the service ID you want to delete: ");
        String idService = Console.ReadLine();
        supplierService.DeleteService(userName, idService);
        userService.DeleteUserService(userName, idService);
      }
      else if (entity.Equals("product"))
      {
        Console.Write("Enter the product ID you want to delete: ");
        String idProcut = Console.ReadLine();
        productService.DeleteProduct(userName,idProcut);
        userService.DeleteUserProduct(userName, idProcut);
      }
    }
    else DisplayPersonalMenu(userName);
  }
}