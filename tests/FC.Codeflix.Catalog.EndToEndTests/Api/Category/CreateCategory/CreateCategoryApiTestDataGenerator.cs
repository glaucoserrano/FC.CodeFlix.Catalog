namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;
public class CreateCategoryApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryApiTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var inputShortName = fixture.getExampleInput();
                    inputShortName.Name = fixture.GetInvalidShortName();
                    invalidInputsList.Add(new object[]{
                        inputShortName,
                        "Name should be at leats 3 characters long"
                     });
                    break;
                case 1:
                    var inputLongName = fixture.getExampleInput();
                    inputLongName.Name = fixture.GetInvalidTooLongName();
                    invalidInputsList.Add(new object[]{
                        inputLongName,
                        "Name should be at less or equal 255 characters long"
                    });
                    break;
                case 2:
                    var inputDescriptionTooLong = fixture.getExampleInput();
                    inputDescriptionTooLong.Description = fixture.GetInvalidTooLongDescription();
                    invalidInputsList.Add(new object[]{
                        inputDescriptionTooLong,
                        "Description should be at less or equal 10000 characters long"
                    });
                    break;

                default:
                    break;
            }
        }
        return invalidInputsList;
    }
}
