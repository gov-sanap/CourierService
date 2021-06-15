# CourierService
 Assignment from Everest Engineering.
 Attached PDF of problem statement.
 ## Following are the points I covered in this solution
 * Console application to calculate DeliveryCost of Package and DeliveryTime of Package.
 * New offers can be added on the fly by using appsettings.json(This can be moved to DB once it's in place).
 * All the weights and distance values can be of type double.
 * Supports Discount types as both Percentage and fixed.
 * Unit test cases are present for both delivery cost and time calculation.


## Instructions to Run Application
### 0. Change current directory to CourierService
`cd CourierService`

### 1. Restore All Dependencies 
`dotnet restore CourierService.sln`

### 2. Build the Solution
`dotnet build CourierService.sln`

### 3. Test the Application
`dotnet test tests\CourierService.Fixture\CourierService.Fixture.csproj`
> Note: For Mac/Linux System please use / slash.
`dotnet test tests/CourierService.Fixture/CourierService.Fixture.csproj`

### 4. User Input
User input file stored in `CourierService\sampleInput.txt` folder.

### 5. Run Application
`dotnet run --project src\DeliveryCostEstimatorCLI\DeliveryCostAndTimeEstimatorCLI.csproj`
> Note: For Mac/Linux System please use / slash.
`dotnet run --project src/DeliveryCostEstimatorCLI/DeliveryCostAndTimeEstimatorCLI.csproj`

### 6. Framework - 
> requires - dotnet-core 2.1 framework
> <br>
> https://dotnet.microsoft.com/download/dotnet-core/2.1
