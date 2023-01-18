namespace ServicesTests.UnitTests.NhlData.MapperTests.Fakes.FakeGameResponse.FakePlayerBioResponse
{
    public class FakePerson
    {
        public int id { get; set; }
        public string fullName { get; set; } = string.Empty;
        public FakePrimaryPosition primaryPosition { get; set; } = new FakePrimaryPosition();
        public FakePersonInfo person { get; set; } = new FakePersonInfo();
    }
}
