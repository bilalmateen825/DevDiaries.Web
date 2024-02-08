using DevDiaries.Web.Models.Blogs;

namespace DevDiaries.Web.Classes
{
    public static class DataUtility
    {
        public static string GetConcatenatedTags(List<Tag> tags)
        {
            string stTag = string.Empty;

            if (tags == null || tags.Count == 0)
                return stTag;

            foreach (Tag tag in tags)
            {
                if (!string.IsNullOrEmpty(stTag))
                    stTag += " ";

                stTag += tag.Name;
            }

            return stTag;
        }

        public static List<string> ParseTagsForUI(List<Tag> tags)
        {
            List<string> lstTag = new List<string>();

            if (tags == null || tags.Count == 0)
                return lstTag;

            foreach (Tag tag in tags)
            {
                lstTag.Add(tag.Name);
            }

            return lstTag;
        }

        public static List<Tag> ParseTags(string stTags)
        {
            if (string.IsNullOrEmpty(stTags))
                return null;

            return new List<Tag>(stTags.Split(' ').Select(tagName => new Tag() { Name = tagName.Trim() }));
        }

        public static bool TagChanged(List<Tag> lstPrevTags, List<Tag> lstNewTags)
        {
            if (lstNewTags == null)
            {
                if (lstPrevTags == null)
                    return false;

                return true;
            }

            foreach (Tag newTag in lstNewTags)
            {
                Tag tag = lstPrevTags.Find(x => x.Name == newTag.Name);

                if (tag == null)
                    return true;
            }

            return false;
        }
    }
}
