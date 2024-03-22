namespace Ocsm.Cofd;

public enum Traits
{
	[Trait(Trait.Category.Social, Trait.Type.Attribute)]
	Composure,
	
	[Trait(Trait.Category.Physical, Trait.Type.Attribute)]
	Dexterity,
	
	[Trait(Trait.Category.Mental, Trait.Type.Attribute)]
	Intelligence,
	
	[Trait(Trait.Category.Social, Trait.Type.Attribute)]
	Manipulation,
	
	[Trait(Trait.Category.Social, Trait.Type.Attribute)]
	Presence,
	
	[Trait(Trait.Category.Mental, Trait.Type.Attribute)]
	Resolve,
	
	[Trait(Trait.Category.Physical, Trait.Type.Attribute)]
	Stamina,
	
	[Trait(Trait.Category.Physical, Trait.Type.Attribute)]
	Strength,
	
	[Trait(Trait.Category.Mental, Trait.Type.Attribute)]
	Wits,
	
	[Trait(Trait.Category.Mental, Trait.Type.Skill)]
	Academics,
	
	[Trait(Trait.Category.Physical, Trait.Type.Skill)]
	Athletics,
	
	[Label("Animal Ken")]
	[Trait(Trait.Category.Social, Trait.Type.Skill)]
	AnimalKen,
	
	[Trait(Trait.Category.Physical, Trait.Type.Skill)]
	Brawl,
	
	[Trait(Trait.Category.Mental, Trait.Type.Skill)]
	Computer,
	
	[Trait(Trait.Category.Mental, Trait.Type.Skill)]
	Crafts,
	
	[Trait(Trait.Category.Physical, Trait.Type.Skill)]
	Drive,
	
	[Trait(Trait.Category.Social, Trait.Type.Skill)]
	Empathy,
	
	[Trait(Trait.Category.Social, Trait.Type.Skill)]
	Expression,
	
	[Trait(Trait.Category.Physical, Trait.Type.Skill)]
	Firearms,
	
	[Trait(Trait.Category.Social, Trait.Type.Skill)]
	Intimidation,
	
	[Trait(Trait.Category.Mental, Trait.Type.Skill)]
	Investigation,
	
	[Trait(Trait.Category.Physical, Trait.Type.Skill)]
	Larceny,
	
	[Trait(Trait.Category.Mental, Trait.Type.Skill)]
	Medicine,
	
	[Trait(Trait.Category.Mental, Trait.Type.Skill)]
	Occult,
	
	[Trait(Trait.Category.Social, Trait.Type.Skill)]
	Persuasion,
	
	[Trait(Trait.Category.Mental, Trait.Type.Skill)]
	Politics,
	
	[Trait(Trait.Category.Mental, Trait.Type.Skill)]
	Science,
	
	[Trait(Trait.Category.Social, Trait.Type.Skill)]
	Socialize,
	
	[Trait(Trait.Category.Physical, Trait.Type.Skill)]
	Stealth,
	
	[Trait(Trait.Category.Social, Trait.Type.Skill)]
	Streetwise,
	
	[Trait(Trait.Category.Social, Trait.Type.Skill)]
	Subterfuge,
	
	[Trait(Trait.Category.Physical, Trait.Type.Skill)]
	Survival,
	
	[Trait(Trait.Category.Physical, Trait.Type.Skill)]
	Weaponry,
}
