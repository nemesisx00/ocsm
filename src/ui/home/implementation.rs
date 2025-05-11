use std::sync::OnceLock;
use glib::subclass::InitializingObject;
use gtk4::{Box, Button, CompositeTemplate, TemplateChild};
use gtk4::glib::{self, clone};
use gtk4::glib::object::ObjectExt;
use gtk4::glib::subclass::Signal;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::types::ObjectSubclassExt;
use gtk4::prelude::{ButtonExt, StaticType};
use gtk4::subclass::box_::BoxImpl;
use gtk4::subclass::prelude::ObjectSubclass;
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};

pub const Signal_NewSheet: &'static str = "newSheet";

#[derive(CompositeTemplate, Default)]
#[template(resource = "/io/github/nemesisx00/OCSM/homeScreen.ui")]
pub struct HomeScreen
{
	#[template_child]
	cofdMortal: TemplateChild<Button>,
	
	#[template_child]
	cofdCtl2e: TemplateChild<Button>,
	
	#[template_child]
	cofdMta2e: TemplateChild<Button>,
	
	#[template_child]
	cofdVtr2e: TemplateChild<Button>,
}

impl BoxImpl for HomeScreen {}

#[glib::object_subclass]
impl ObjectSubclass for HomeScreen
{
	// `NAME` needs to match `class` attribute of template
	const NAME: &'static str = "HomeScreen";
	type Type = super::HomeScreen;
	type ParentType = Box;
	
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
		
		self.cofdMortal.connect_clicked(clone!(#[weak] me, move |_| me.emitNewSheet(0)));
		self.cofdCtl2e.connect_clicked(clone!(#[weak] me, move |_| me.emitNewSheet(1)));
		self.cofdMta2e.connect_clicked(clone!(#[weak] me, move |_| me.emitNewSheet(2)));
		self.cofdVtr2e.connect_clicked(clone!(#[weak] me, move |_| me.emitNewSheet(3)));
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

impl HomeScreen
{
	fn emitNewSheet(&self, gameSystem: u32)
	{
		self.obj().emit_by_name::<()>(
			Signal_NewSheet,
			&[&gameSystem]
		);
	}
}
