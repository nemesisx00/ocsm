mod attributes;
pub mod data;
mod info;
mod merit;
mod skills;
mod sheet;
mod tracks;
mod traits;

pub use attributes::{AttributesMentalElement, AttributesPhysicalElement, AttributesSocialElement};
pub use merit::MeritElement;
pub use skills::{SkillsMentalElement, SkillsPhysicalElement, SkillsSocialElement};
pub use sheet::SheetElement as CofdSheetElement;

pub const LabelMinWidth: usize = 110;
