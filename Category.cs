namespace Recipe_Rack
{
	public class RecipeCategory
	{
		public string categoryName { get; set; }

		public RecipeCategory(string RecipeCategoryName)
		{
			categoryName = RecipeCategoryName;
		}

		public RecipeCategory() { }
	}
}