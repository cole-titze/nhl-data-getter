namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerBioResponse
{
    public class FakePlayerBioData
    {
        public FakePerson[] people { get; set; } = new FakePerson[] { new FakePerson() };
        public FakePerson?[] roster { get; set; } = new FakePerson[] { new FakePerson() };

    }
}
