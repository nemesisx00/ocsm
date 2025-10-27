mod attributes;
pub mod data;
mod info;
mod merit;
mod nodes;
mod skills;
mod sheet;
mod tracks;
mod traits;

pub use attributes::{AttributesMentalElement, AttributesPhysicalElement,
	AttributesSocialElement};
pub use merit::MeritElement;
pub use nodes::list::SheetList as CofdSheetList;
pub use skills::{SkillsMentalElement, SkillsPhysicalElement, SkillsSocialElement,
	SkillSpecialtyListElement};
pub use sheet::SheetElement as CofdSheetElement;

pub const LabelMinWidth: usize = 110;
