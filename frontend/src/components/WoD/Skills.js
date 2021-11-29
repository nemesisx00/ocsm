import './Skills.css'
import React from 'react'
import Tracker from './Tracker'

export default class Skills extends React.Component
{
	render()
	{
		return (
			<div className="skills">
				<div className="row">
					<div className="column">
						<div className="label">Academics:</div>
						<div className="label">Computer:</div>
						<div className="label">Crafts:</div>
						<div className="label">Investigation:</div>
						<div className="label">Medicine:</div>
						<div className="label">Occult:</div>
						<div className="label">Politics:</div>
						<div className="label">Science:</div>
					</div>
					<div className="column">
						<Tracker keyWord="academics" className="spacer dots academics" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.academics} changeHandler={(arg) => this.props.changeHandler(arg, 'academics')} />
						<Tracker keyWord="computer" className="spacer dots computer" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.computer} changeHandler={(arg) => this.props.changeHandler(arg, 'computer')} />
						<Tracker keyWord="crafts" className="spacer dots crafts" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.crafts} changeHandler={(arg) => this.props.changeHandler(arg, 'crafts')} />
						<Tracker keyWord="investigation" className="spacer dots investigation" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.investigation} changeHandler={(arg) => this.props.changeHandler(arg, 'investigation')} />
						<Tracker keyWord="medicine" className="spacer dots medicine" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.medicine} changeHandler={(arg) => this.props.changeHandler(arg, 'medicine')} />
						<Tracker keyWord="occult" className="spacer dots occult" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.occult} changeHandler={(arg) => this.props.changeHandler(arg, 'occult')} />
						<Tracker keyWord="politics" className="spacer dots politics" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.politics} changeHandler={(arg) => this.props.changeHandler(arg, 'politics')} />
						<Tracker keyWord="science" className="spacer dots science" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.science} changeHandler={(arg) => this.props.changeHandler(arg, 'science')} />
					</div>
				</div>
				<div className="row">
					<div className="column">
						<div className="label">Athletics:</div>
						<div className="label">Brawl:</div>
						<div className="label">Drive:</div>
						<div className="label">Firearms:</div>
						<div className="label">Larceny:</div>
						<div className="label">Stealth:</div>
						<div className="label">Survival:</div>
						<div className="label">Weaponry:</div>
					</div>
					<div className="column">
						<Tracker keyWord="athletics" className="spacer dots athletics" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.athletics} changeHandler={(arg) => this.props.changeHandler(arg, 'athletics')} />
						<Tracker keyWord="brawl" className="spacer dots brawl" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.brawl} changeHandler={(arg) => this.props.changeHandler(arg, 'brawl')} />
						<Tracker keyWord="drive" className="spacer dots drive" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.drive} changeHandler={(arg) => this.props.changeHandler(arg, 'drive')} />
						<Tracker keyWord="firearms" className="spacer dots firearms" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.firearms} changeHandler={(arg) => this.props.changeHandler(arg, 'firearms')} />
						<Tracker keyWord="larceny" className="spacer dots larceny" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.larceny} changeHandler={(arg) => this.props.changeHandler(arg, 'larceny')} />
						<Tracker keyWord="stealth" className="spacer dots stealth" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.stealth} changeHandler={(arg) => this.props.changeHandler(arg, 'stealth')} />
						<Tracker keyWord="survival" className="spacer dots survival" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.survival} changeHandler={(arg) => this.props.changeHandler(arg, 'survival')} />
						<Tracker keyWord="weaponry" className="spacer dots weaponry" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.weaponry} changeHandler={(arg) => this.props.changeHandler(arg, 'weaponry')} />
					</div>
				</div>
				<div className="row">
					<div className="column">
						<div className="label">Animal Ken:</div>
						<div className="label">Empathy:</div>
						<div className="label">Expression:</div>
						<div className="label">Intimidation:</div>
						<div className="label">Persuasion:</div>
						<div className="label">Socialize:</div>
						<div className="label">Streetwise:</div>
						<div className="label">Subterfuge:</div>
					</div>
					<div className="column">
						<Tracker keyWord="animalKen" className="spacer dots animalKen" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.animalKen} changeHandler={(arg) => this.props.changeHandler(arg, 'animalKen')} />
						<Tracker keyWord="empathy" className="spacer dots empathy" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.empathy} changeHandler={(arg) => this.props.changeHandler(arg, 'empathy')} />
						<Tracker keyWord="expression" className="spacer dots expression" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.expression} changeHandler={(arg) => this.props.changeHandler(arg, 'expression')} />
						<Tracker keyWord="intimidation" className="spacer dots intimidation" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.intimidation} changeHandler={(arg) => this.props.changeHandler(arg, 'intimidation')} />
						<Tracker keyWord="persuasion" className="spacer dots persuasion" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.persuasion} changeHandler={(arg) => this.props.changeHandler(arg, 'persuasion')} />
						<Tracker keyWord="socialize" className="spacer dots socialize" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.socialize} changeHandler={(arg) => this.props.changeHandler(arg, 'socialize')} />
						<Tracker keyWord="streetwise" className="spacer dots streetwise" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.streetwise} changeHandler={(arg) => this.props.changeHandler(arg, 'streetwise')} />
						<Tracker keyWord="subterfuge" className="spacer dots subterfuge" type={Tracker.Types.Circle} max={this.props.max} value={this.props.skills.subterfuge} changeHandler={(arg) => this.props.changeHandler(arg, 'subterfuge')} />
					</div>
				</div>
			</div>
		)
	}
}
