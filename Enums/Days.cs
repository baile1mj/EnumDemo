namespace Enums
{
    /// <summary>
    /// Contains an enumeration of the days of the week.
    /// </summary>
    public sealed class Days
    {
        private readonly int _position;

        /// <summary>
        /// Gets the name of the day.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new day to include in the enumeration.
        /// </summary>
        /// <param name="name">The name of the day.</param>
        /// <param name="position">The position of the day relative to other days in the week.</param>
        private Days(string name, int position)
        {
            Name = name;
            _position = position;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Gets the number of days from the current day until the next instance of the specified day.
        /// </summary>
        /// <param name="next">The day at which to stop counting the number of days.</param>
        /// <returns>The number of days from the current day until the next occurrence of the specified day.</returns>
        public int DaysUntil(Days next)
        {
            // Add a week to prevent a negative the mod to get the actual number of days.
            return (next._position + 7 - _position) % 7;
        }
        
        public static readonly Days Sunday = new Days("Sunday", 0);
        public static readonly Days Monday = new Days("Monday", 1);
        public static readonly Days Tuesday = new Days("Tuesday", 2);
        public static readonly Days Wednesday = new Days("Wednesday",3 );
        public static readonly Days Thursday = new Days("Thursday", 4);
        public static readonly Days Friday = new Days("Friday", 5);
        public static readonly Days Saturday = new Days("Saturday", 6);

        /// <summary>
        /// Enumerates all the days in the week.
        /// </summary>
        public static IEnumerable<Days> Values
        {
            get
            {
                yield return Sunday;
                yield return Monday;
                yield return Tuesday;
                yield return Wednesday;
                yield return Thursday;
                yield return Friday;
                yield return Saturday;
            }
        }

        /// <summary>
        /// Gets a <see cref="Days"/> value from its string representation.
        /// </summary>
        /// <param name="day">The day string to parse.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If the provided string is null or empty.</exception>
        /// <exception cref="IndexOutOfRangeException">If the provided string does not represent one of the days in the enumeration.</exception>
        public static Days Parse(string day)
        {
            if (string.IsNullOrEmpty(day))
            {
                throw new ArgumentNullException(nameof(day), "You must specify a day name to parse.");
            }
            
            foreach (var value in Values)
            {
                if (day == value.Name) { return value; }
            }

            throw new IndexOutOfRangeException($"\"{day}\" is not a valid day.");
        }

        /// <summary>
        /// Counts the number of days elapsed in a daily timespan, which does not include the
        /// first day.  For example, only 2 days will elapse in the time span from Monday
        /// to Wednesday.
        /// </summary>
        /// <param name="from">The day on which the span starts.</param>
        /// <param name="to">The day on which the span ends.</param>
        /// <returns>The number of days elapsed in a time span.</returns>
        public static int CountElapsed(Days from, Days to)
        {
            return from.DaysUntil(to);
        }
    }
}