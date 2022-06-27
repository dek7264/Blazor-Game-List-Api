using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace Game_List_Api.Tests.Unit
{
    public static class AMatch
    {
        public static T Of<T>(T expected)
        {
            return Of<T, T>(expected);
        }

        public static T Of<T>(T expected, Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
        {
            return Of<T, T>(expected, config);
        }

        public static TActual Of<TExpected, TActual>(TExpected expected)
        {
            return Of<TExpected, TActual>(expected, config => config);
        }

        public static TActual Of<TExpected, TActual>(TExpected expected, Func<EquivalencyAssertionOptions<TExpected>, EquivalencyAssertionOptions<TExpected>> config)
        {
            // ReSharper disable once FakeItEasy0003
            return A<TActual>.That.Matches(actual => Matches(expected, actual, config));
        }

        private static bool Matches<TExpected, TActual>(TExpected expected, TActual actual,
            Func<EquivalencyAssertionOptions<TExpected>, EquivalencyAssertionOptions<TExpected>> config)
        {
            using var scope = new AssertionScope();
            try
            {
                actual.Should().BeEquivalentTo(expected, config);
                return !scope.HasFailures();
            }
            finally
            {
                foreach (var error in scope.Discard())
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}