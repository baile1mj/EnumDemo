using Enums;
using System.Reflection;

namespace EnumsTests
{
    public class DaysShould
    {
        [Fact]
        public void EnumerateAllDefinedValues()
        {
            var definedDays = typeof(Days)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(info => info.FieldType == typeof(Days))
                .Select(info => (Days)info.GetValue(null)!)
                .ToList();
            var enumeratedDays = Days.Values.ToList();
            var notEnumerated = definedDays.Where(d => !enumeratedDays.Contains(d));
            var unexpectedDays = enumeratedDays.Where(d => !definedDays.Contains(d));

            Assert.False(notEnumerated.Any());
            Assert.False(unexpectedDays.Any());
        }

        [Fact]
        public void ParseAllDefinedDayNames()
        {
            var days = Days.Values.ToList();

            foreach (var expected in days)
            {
                var actual = Days.Parse(expected.Name);

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void PreventParsingInvalidDayNames()
        {
            var emptyNameError = TryInvoke(() => Days.Parse(""));
            var nullNameError = TryInvoke(() => Days.Parse(null));
            var undefinedNameError = TryInvoke(() => Days.Parse("Thor's Day"));
            var caseMismatchError = TryInvoke(() => Days.Parse(Days.Monday.Name.ToLower()));

            Assert.NotNull(emptyNameError);
            Assert.NotNull(nullNameError);
            Assert.NotNull(undefinedNameError);
            Assert.NotNull(caseMismatchError);
        }

        [Fact]
        public void CorrectlyCalculateDaysUntilNext()
        {
            var mondayToMonday = Days.Monday.DaysUntil(Days.Monday);
            Assert.Equal(0, mondayToMonday);

            var mondayToTuesday = Days.Monday.DaysUntil(Days.Tuesday);
            Assert.Equal(1, mondayToTuesday);

            var mondayToSunday = Days.Monday.DaysUntil(Days.Sunday);
            Assert.Equal(6, mondayToSunday);

            var fridayToTuesday = Days.Friday.DaysUntil(Days.Tuesday);
            Assert.Equal(4, fridayToTuesday);
        }

        [Fact]
        public void CorrectlyCalculateDaysElapsed()
        {
            var mondayToMonday = Days.CountElapsed(Days.Monday, Days.Monday);
            Assert.Equal(0, mondayToMonday);

            var mondayToTuesday = Days.CountElapsed(Days.Monday, Days.Tuesday);
            Assert.Equal(1, mondayToTuesday);

            var mondayToSunday = Days.CountElapsed(Days.Monday, Days.Sunday);
            Assert.Equal(6, mondayToSunday);

            var fridayToTuesday = Days.CountElapsed(Days.Friday, Days.Tuesday);
            Assert.Equal(4, fridayToTuesday);
        }

        [Fact]
        public void UseNameAsStringRepresentation()
        {
            foreach (var value in Days.Values)
            {
                Assert.Equal(value.Name, value.ToString());
            }
        }

        private static Exception? TryInvoke(Action method)
        {
            try
            {
                method.Invoke();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}