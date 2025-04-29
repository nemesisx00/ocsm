use std::cell::Cell;
use std::path::Path;
use std::sync::OnceLock;
use gtk4::glib::subclass::{InitializingObject, Signal};
use gtk4::{BinLayout, CompositeTemplate, GestureClick, Image, TemplateChild, Widget};
use gtk4::glib::{self, clone, Properties};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::prelude::{GestureExt, StaticType, WidgetExt};
use gtk4::subclass::prelude::DerivedObjectProperties;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use super::mode::StatefulMode;

pub const Signal_StateToggled: &'static str = "stateToggled";

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

#[derive(CompositeTemplate, Default, Properties)]
#[properties(wrapper_type = super::StatefulButton)]
#[template(resource = "/io/github/nemesisx00/OCSM/widgets/statefulButton.ui")]
pub struct StatefulButton
{
	#[property(construct, default = StatefulMode::CircleOne.into(), get, set = StatefulButton::setMode)]
	mode: Cell<u32>,
	
	#[property(construct, get, set)]
	index: Cell<u32>,
	
	#[property(construct, default = 0, get, set)]
	state: Cell<u32>,
	
	#[template_child]
	fill: TemplateChild<Image>,
	
	#[template_child]
	outline: TemplateChild<Image>,
}

#[glib::derived_properties]
impl ObjectImpl for StatefulButton
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		let assetDir = env!("CARGO_MANIFEST_DIR");
		let obj = self.obj();
		
		obj.set_cursor_from_name(Some("pointer"));
		obj.add_css_class("stateful");
		obj.set_size_request(14, 14);
		
		self.outline.set_from_file(Some(Path::new(format!(
			"{}/../../assets/{}",
			assetDir,
			CircleEmpty
		).as_str())));
		
		// Connect a gesture to handle clicks.
		let gesture = GestureClick::new();
		let me = self;
		gesture.connect_released(clone!(#[weak] me, move |gesture, _, _, _| me.handleClick(gesture)));
		obj.add_controller(gesture);
		
		self.updateImage();
	}
	
	fn dispose(&self)
	{
		self.fill.unparent();
		self.outline.unparent();
	}
	
	fn signals() -> &'static [Signal]
	{
		static SIGNALS: OnceLock<Vec<Signal>> = OnceLock::new();
		return SIGNALS.get_or_init(|| {
			vec![
				Signal::builder(Signal_StateToggled)
					.param_types([u32::static_type()])
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
		klass.bind_template();
		klass.set_accessible_role(gtk4::AccessibleRole::Button);
		klass.set_css_name("button");
		klass.set_layout_manager_type::<BinLayout>();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl StatefulButton
{
	fn handleClick(&self, gesture: &GestureClick)
	{
		gesture.set_state(gtk4::EventSequenceState::Claimed);
		self.incrementState();
		self.updateImage();
		
		self.obj().emit_by_name::<()>(
			Signal_StateToggled,
			&[&self.index.get()]
		);
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
				1 => SlashTwo,
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
		match self.mode.get().into()
		{
			StatefulMode::CircleOne | StatefulMode::CircleThree => {
				let assetDir = env!("CARGO_MANIFEST_DIR");
				self.outline.set_from_file(Some(Path::new(format!(
					"{}/../../assets/{}",
					assetDir,
					CircleEmpty
				).as_str())));
			},
			
			_ => {
				let assetDir = env!("CARGO_MANIFEST_DIR");
				self.outline.set_from_file(Some(Path::new(format!(
					"{}/../../assets/{}",
					assetDir,
					BoxBorder
				).as_str())));
			}
		}
		
		let name = self.getImageName();
		if name == CircleEmpty || name == BoxEmpty
		{
			self.fill.clear();
		}
		else
		{
			let assetDir = env!("CARGO_MANIFEST_DIR");
			self.fill.set_from_file(Some(Path::new(format!(
				"{}/../../assets/{}",
				assetDir,
				name
			).as_str())));
		}
	}
}
