use std::sync::OnceLock;
use glib::subclass::InitializingObject;
use gtk4::glib::clone;
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::Signal;
use gtk4::prelude::{ButtonExt, StaticType, WidgetExt};
use gtk4::subclass::prelude::*;
use gtk4::{glib, Button, CompositeTemplate};

pub const Signal_NewSheet: &'static str = "newSheet";

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/homeScreen.ui")]
pub struct HomeScreen
{
	#[template_child]
	cofdMortal: TemplateChild<Button>,
}

impl BoxImpl for HomeScreen {}

#[glib::object_subclass]
impl ObjectSubclass for HomeScreen
{
	// `NAME` needs to match `class` attribute of template
	const NAME: &'static str = "HomeScreen";
	type Type = super::HomeScreen;
	type ParentType = gtk4::Box;
	
	fn class_init(klass: &mut Self::Class)
	{
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl ObjectImpl for HomeScreen
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		let me = self;
		
		self.cofdMortal.connect_clicked(clone!(#[weak] me, move |_| {
			me.obj().emit_by_name::<()>(
				Signal_NewSheet,
				&[&0u32]
			);
		}));
	}
	
	fn dispose(&self)
	{
		self.cofdMortal.unparent();
	}
	
	fn signals() -> &'static [Signal]
	{
		static SIGNALS: OnceLock<Vec<Signal>> = OnceLock::new();
		return SIGNALS.get_or_init(|| {
			vec![
				Signal::builder(Signal_NewSheet)
					.param_types([u32::static_type()])
					.build()
			]
		});
	}
}

impl WidgetImpl for HomeScreen {}
