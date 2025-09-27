use gtk4::glib::object::Cast;
use gtk4::prelude::{GridExt, ObjectExt, WidgetExt};
use gtk4::subclass::grid::GridImpl;
use gtk4::{CompositeTemplate, Grid, Label, TemplateChild};
use gtk4::glib::{self, closure_local, GString, Properties};
use gtk4::glib::subclass::InitializingObject;
use gtk4::glib::subclass::object::{ObjectImpl, ObjectImplExt};
use gtk4::glib::subclass::prelude::DerivedObjectProperties;
use gtk4::glib::subclass::types::{ObjectSubclass, ObjectSubclassExt};
use gtk4::subclass::widget::{CompositeTemplateClass, CompositeTemplateInitializingExt, WidgetClassExt, WidgetImpl};
use crate::widgets::list::dotLabel::DotLabel;

#[derive(CompositeTemplate, Default, Properties)]
#[properties(wrapper_type = super::DotLabelList)]
#[template(resource = "/io/github/nemesisx00/OCSM/cofd/dotLabelList.ui")]
pub struct DotLabelList
{
	#[property(
		construct,
		default = "List",
		get = Self::labelText,
		name = "label",
		set = Self::setLabelText,
		type = GString,
	)]
	#[template_child]
	label: TemplateChild<Label>,
}

impl GridImpl for DotLabelList {}

#[glib::derived_properties]
impl ObjectImpl for DotLabelList
{
	fn constructed(&self)
	{
		self.parent_constructed();
		
		self.refresh();
	}
}

#[glib::object_subclass]
impl ObjectSubclass for DotLabelList
{
	const NAME: &'static str = "DotLabelList";
	type ParentType = Grid;
	type Type = super::DotLabelList;
	
	fn class_init(klass: &mut Self::Class)
	{
		klass.bind_template();
	}
	
	fn instance_init(obj: &InitializingObject<Self>)
	{
		obj.init_template();
	}
}

impl WidgetImpl for DotLabelList {}

impl DotLabelList
{
	fn generateDotLabel(&self, label: String, value: u32) -> DotLabel
	{
		let me = self;
		let empty = DotLabel::new();
		empty.set_label(label);
		empty.set_value(value);
		
		empty.connect_closure(DotLabel::Signal_DotLabelChanged, false, closure_local!(
			#[weak] me,
			move |_: DotLabel| me.refresh()
		));
		
		return empty;
	}
	
	fn labelText(&self) -> GString
	{
		return self.label.label();
	}
	
	pub fn refresh(&self)
	{
		let mut values = self.valuesRemove(true);
		values.push((String::default(), 0));
		
		let mut column;
		let mut row;
		for (i, (label, value)) in values.iter().enumerate()
		{
			let dotLabel = self.generateDotLabel(label.clone(), *value);
			
			column = i % 2 * 3;
			row = 1 + i / 2;
			
			self.obj().attach(&dotLabel, column as i32, row as i32, 3, 1);
		}
	}
	
	fn setLabelText(&self, label: GString)
	{
		self.label.set_label(label.as_str());
	}
	
	pub fn values(&self) -> Vec<(String, u32)>
	{
		return self.valuesRemove(false);
	}
	
	fn valuesRemove(&self, remove: bool) -> Vec<(String, u32)>
	{
		let mut values = vec![];
		if let Some(firstChild) = self.obj().first_child()
		{
			let mut child = firstChild.next_sibling();
			while child.is_some()
			{
				let widget = child.unwrap();
				
				if let Ok(dotLabel) = widget.clone().downcast::<DotLabel>()
				{
					if !dotLabel.isEmpty()
					{
						values.push((dotLabel.label(), dotLabel.value()));
					}
				}
				
				child = widget.next_sibling();
				
				if remove
				{
					self.obj().remove(&widget);
				}
			}
		}
		
		return values;
	}
}
