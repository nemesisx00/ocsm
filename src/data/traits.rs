use anyhow::Result;

pub trait CharacterSheet
{
	fn save(&self) -> Result<String>;
	fn update(&mut self, other: &Self);
}
