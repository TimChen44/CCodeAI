using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SkillsProvider : ISkillsProvider
{
    ///<inheritdoc/>
    public string SkillsLocation { get; }

    public string DefaultDir { get; }

    public SkillsProvider()
    {
        SkillsLocation = Path.Combine(
            Path.GetDirectoryName(typeof(SkillsProvider).Assembly.Location),
            "Skills");
    }

    public SkillsProvider(string skilldir,string defaultDir = null)
    {
        SkillsLocation = skilldir;
        DefaultDir = defaultDir;
    }

    public IEnumerable<SkillModel> GetSkills()
    {
        if (!Directory.Exists(SkillsLocation))
        {
            throw new DirectoryNotFoundException($"Skill NotFound：{SkillsLocation}");
        }

        var skillDirs = Directory.GetDirectories(SkillsLocation)
            .Where(p => p.EndsWith("Skill"));

        if (!string.IsNullOrEmpty(DefaultDir) && Directory.Exists(DefaultDir))
        {
            skillDirs = Directory.GetDirectories(DefaultDir)
                .Where(p => p.EndsWith("Skill"))
                .Concat(skillDirs);
        }

        int index = 1;
        foreach (var dir in skillDirs)
        {
            var model = new SkillModel(dir);
            model.Index = index++;
            yield return model;
        }
    }
}