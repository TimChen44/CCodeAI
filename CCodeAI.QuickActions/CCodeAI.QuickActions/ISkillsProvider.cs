using System.Collections.Generic;

public interface ISkillsProvider
{
    /// <summary>
    /// default directory which save skills
    /// </summary>
    string SkillsLocation { get; }

    IEnumerable<SkillModel> GetSkills();
}

