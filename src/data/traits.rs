use anyhow::Result;

pub trait CharacterSheet
{
	fn serialize(&self) -> Result<String>;
	fn update(&mut self, other: &Self);
}
