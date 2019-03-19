using System.ComponentModel;

namespace Po.Forms.PropertyGrid
{
    /// <summary>
    /// <see cref="CategoryAttribute"/> with the added functionality of sorting.
    /// </summary>
    public class SortedCategoryAttribute : CategoryAttribute
    {
        private const char NonPrintableChar = '\t';

        /// <param name="category">Category name.</param>
        /// <param name="categoryPosition">Position from end.</param>
        public SortedCategoryAttribute(
            string category, ushort categoryPosition)
            : base(category.PadLeft(category.Length + categoryPosition, NonPrintableChar)) { }
    }
}
