use std::cell::{Cell, RefCell};
use std::sync::OnceLock;
use gtk4::glib::subclass::Signal;
use gtk4::{BinLayout, GestureClick, Image, Widget};
use gtk4::glib::{self, clone, Properties};
use gtk4::glib::object::{Cast, CastNone, ObjectExt};
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::prelude::{GestureExt, WidgetExt};
use gtk4::subclass::prelude::DerivedObjectProperties;
use gtk4::subclass::widget::{WidgetClassExt, WidgetImpl};

pub const StatefulButton_Mode: &'static str = "mode";
pub const StatefulButton_State: &'static str = "state";

const CircleEmpty: &'static str = "circle-empty.png";
const CircleFull: &'static str = "circle-fill.png";
const CircleHalf: &'static str = "circle-fill-half.png";
const CircleRed: &'static str = "circle-fill-red.png";
const BoxBorder: &'static str = "box-border-16.png";
const BoxEmpty: &'static str = "box-transparent-16.png";
const SlashOne: &'static str = "slash-one.png";
const SlashTwo: &'static str = "slash-two.png";
const SlashThree: &'static str = "slash-three.png";

#[derive(Copy, Clone)]
pub enum StatefulMode
{
	CircleOne,
	CircleThree,
	BoxOne,
	BoxTwo,
	BoxThree,
}

impl From<u32> for StatefulMode
{
	fn from(value: u32) -> Self
	{
		return match value
		{
			1 => StatefulMode::CircleThree,
			2 => StatefulMode::BoxOne,
			3 => StatefulMode::BoxTwo,
			4 => StatefulMode::BoxThree,
			_ => StatefulMode::CircleOne,
		};
	}
}

impl From<StatefulMode> for u32
{
	fn from(value: StatefulMode) -> Self
	{
		return match value
		{
			StatefulMode::CircleThree => 1,
			StatefulMode::BoxOne => 2,
			StatefulMode::BoxTwo => 3,
			StatefulMode::BoxThree => 4,
			_ => 0,
		};
	}
}

impl StatefulMode
{
	pub fn getMaxStates(mode: Self) -> u32
	{
		return match mode
		{
			StatefulMode::CircleOne => 2,
			StatefulMode::CircleThree => 4,
			StatefulMode::BoxOne => 2,
			StatefulMode::BoxTwo => 3,
			StatefulMode::BoxThree => 4,
		};
	}
}

#[derive(Default, Properties)]
#[properties(wrapper_type = super::StatefulButton)]
pub struct StatefulButton
{
	#[property(construct, default = StatefulMode::CircleOne.into(), get, set = StatefulButton::setMode)]
	mode: Cell<u32>,
	
	#[property(construct, default = 0, get, set)]
	state: Cell<u32>,
	
	outline: RefCell<Option<Widget>>,
	fill: RefCell<Option<Widget>>,
}

#[glib::derived_properties]
impl ObjectImpl for StatefulButton
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.obj().set_cursor_from_name(Some("pointer"));
		self.obj().add_css_class("stateful");
		self.obj().set_margin_bottom(0);
		self.obj().set_margin_end(0);
		self.obj().set_margin_start(0);
		self.obj().set_margin_top(0);
		self.obj().set_size_request(14, 14);
		
        let obj = self.obj();
		let assetDir = env!("CARGO_MANIFEST_DIR");
		
		let outline = Image::builder()
			.hexpand(false)
			.margin_bottom(0)
			.margin_end(0)
			.margin_start(0)
			.margin_top(0)
			.vexpand(false)
			.file(format!(
				"{}/../../assets/{}",
				assetDir,
				CircleEmpty
			))
			.build();
		
		outline.set_parent(&*obj);
		*self.outline.borrow_mut() = Some(outline.upcast::<Widget>());
		
		let fill = Image::builder()
			.hexpand(false)
			.margin_bottom(0)
			.margin_end(0)
			.margin_start(0)
			.margin_top(0)
			.vexpand(false)
			.build();
		
		fill.set_parent(&*obj);
		*self.fill.borrow_mut() = Some(fill.upcast::<Widget>());
		
		// Connect a gesture to handle clicks.
		let gesture = GestureClick::new();
		let me = self;
		gesture.connect_released(clone!(#[weak] me, move |gesture, _, _, _| me.handleClick(gesture)));
		obj.add_controller(gesture);
		
		self.updateImage();
	}
	
	fn dispose(&self)
	{
		// Child widgets need to be manually un-parented in `dispose()`.
		if let Some(child) = self.outline.borrow_mut().take()
		{
			child.unparent();
		}
		
		if let Some(child) = self.fill.borrow_mut().take()
		{
			child.unparent();
		}
	}
	
	fn signals() -> &'static [Signal]
	{
		static SIGNALS: OnceLock<Vec<Signal>> = OnceLock::new();
		return SIGNALS.get_or_init(|| {
			vec![
				Signal::builder("stateToggled")
					.build()
			]
		});
	}
}

impl WidgetImpl for StatefulButton {}

#[glib::object_subclass]
impl ObjectSubclass for StatefulButton
{
	const NAME: &'static str = "StatefulButton";
	type ParentType = Widget;
	type Type = super::StatefulButton;
	
	fn class_init(klass: &mut Self::Class)
	{
		klass.set_accessible_role(gtk4::AccessibleRole::Button);
		klass.set_css_name("button");
		klass.set_layout_manager_type::<BinLayout>();
	}
}

impl StatefulButton
{
	fn handleClick(&self, gesture: &GestureClick)
	{
		gesture.set_state(gtk4::EventSequenceState::Claimed);
		self.incrementState();
		self.updateImage();
		
		self.obj().emit_by_name::<()>("stateToggled", &[]);
	}
	
	fn incrementState(&self)
	{
		self.state.set(
			(self.state.get() + 1)
			% StatefulMode::getMaxStates(self.mode.get().into())
		);
	}
	
	fn setMode(&self, mode: u32)
	{
		self.mode.set(mode);
		self.updateImage();
	}
	
	fn getImageName(&self) -> &str
	{
		return match self.mode.get().into()
		{
			StatefulMode::CircleThree => match self.state.get()
			{
				1 => CircleHalf,
				2 => CircleFull,
				3 => CircleRed,
				_ => CircleEmpty,
			},
			
			StatefulMode::BoxOne => match self.state.get()
			{
				1 => SlashOne,
				_ => BoxEmpty,
			},
			
			StatefulMode::BoxTwo => match self.state.get()
			{
				1 => SlashOne,
				2 => SlashTwo,
				_ => BoxEmpty,
			},
			
			StatefulMode::BoxThree => match self.state.get()
			{
				1 => SlashOne,
				2 => SlashTwo,
				3 => SlashThree,
				_ => BoxEmpty,
			},
			
			//StatefulMode::CircleOne
			_ => match self.state.get()
			{
				1 => CircleFull,
				_ => CircleEmpty,
			},
		};
	}
	
	pub fn updateImage(&self)
	{
		if let Some(i) = self.outline.borrow_mut().and_downcast_ref::<Image>()
		{
			match self.mode.get().into()
			{
				StatefulMode::CircleOne | StatefulMode::CircleThree => {
					let assetDir = env!("CARGO_MANIFEST_DIR");
					i.set_from_file(Some(format!(
						"{}/../../assets/{}",
						assetDir,
						CircleEmpty
					)));
				},
				
				_ => {
					let assetDir = env!("CARGO_MANIFEST_DIR");
					i.set_from_file(Some(format!(
						"{}/../../assets/{}",
						assetDir,
						BoxBorder
					)));
				}
			}
		}
		
		if let Some(i) = self.fill.borrow_mut().and_downcast_ref::<Image>()
		{
			let name = self.getImageName();
			if name == CircleEmpty || name == BoxEmpty
			{
				i.clear();
			}
			else
			{
				let assetDir = env!("CARGO_MANIFEST_DIR");
				i.set_from_file(Some(format!(
					"{}/../../assets/{}",
					assetDir,
					name
				)));
			}
		}
	}
}
