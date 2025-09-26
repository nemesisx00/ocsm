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
		
		self.cofdMortal.connect_clicked(clone!(#[weak] me, move |_| me.emitNewSheet(cofd::GameSystemId.into())));
		self.cofdCtl2e.connect_clicked(clone!(#[weak] me, move |_| me.emitNewSheet(ctl2e::GameSystemId.into())));
		self.cofdMta2e.connect_clicked(clone!(#[weak] me, move |_| me.emitNewSheet(mta2e::GameSystemId.into())));
		self.cofdVtr2e.connect_clicked(clone!(#[weak] me, move |_| me.emitNewSheet(vtr2e::GameSystemId.into())));
	}
	
	fn signals() -> &'static [Signal]
	{
		static SIGNALS: OnceLock<Vec<Signal>> = OnceLock::new();
		return SIGNALS.get_or_init(|| {
			return vec![
				Signal::builder(Signal_NewSheet)
					.param_types([String::static_type()])
					.build()
			];
		});
	}
}

impl WidgetImpl for HomeScreen {}

impl HomeScreen
{
	fn emitNewSheet(&self, gameSystem: String)
	{
		self.obj().emit_by_name::<()>(
			Signal_NewSheet,
			&[&gameSystem]
		);
	}
}
