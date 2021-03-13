import React from 'react'
import FiveDots from '../FiveDots'
import './Skills.css'

export default class Skills extends React.Component
{
	render()
	{
		return (
			<div className="skills">
				<div className="mental">
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
						<FiveDots className="dots academics" value={this.props.skills.academics} valueChangedHandler={(value) => this.props.changeHandler(value, 'academics')} />
						<FiveDots className="dots computer" value={this.props.skills.computer} valueChangedHandler={(value) => this.props.changeHandler(value, 'computer')} />
						<FiveDots className="dots crafts" value={this.props.skills.crafts} valueChangedHandler={(value) => this.props.changeHandler(value, 'crafts')} />
						<FiveDots className="dots investigation" value={this.props.skills.investigation} valueChangedHandler={(value) => this.props.changeHandler(value, 'investigation')} />
						<FiveDots className="dots medicine" value={this.props.skills.medicine} valueChangedHandler={(value) => this.props.changeHandler(value, 'medicine')} />
						<FiveDots className="dots occult" value={this.props.skills.occult} valueChangedHandler={(value) => this.props.changeHandler(value, 'occult')} />
						<FiveDots className="dots politics" value={this.props.skills.politics} valueChangedHandler={(value) => this.props.changeHandler(value, 'politics')} />
						<FiveDots className="dots science" value={this.props.skills.science} valueChangedHandler={(value) => this.props.changeHandler(value, 'science')} />
					</div>
				</div>
				<div className="physical">
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
						<FiveDots className="dots athletics" value={this.props.skills.athletics} valueChangedHandler={(value) => this.props.changeHandler(value, 'athletics')} />
						<FiveDots className="dots brawl" value={this.props.skills.brawl} valueChangedHandler={(value) => this.props.changeHandler(value, 'brawl')} />
						<FiveDots className="dots drive" value={this.props.skills.drive} valueChangedHandler={(value) => this.props.changeHandler(value, 'drive')} />
						<FiveDots className="dots firearms" value={this.props.skills.firearms} valueChangedHandler={(value) => this.props.changeHandler(value, 'firearms')} />
						<FiveDots className="dots larceny" value={this.props.skills.larceny} valueChangedHandler={(value) => this.props.changeHandler(value, 'larceny')} />
						<FiveDots className="dots stealth" value={this.props.skills.stealth} valueChangedHandler={(value) => this.props.changeHandler(value, 'stealth')} />
						<FiveDots className="dots survival" value={this.props.skills.survival} valueChangedHandler={(value) => this.props.changeHandler(value, 'survival')} />
						<FiveDots className="dots weaponry" value={this.props.skills.weaponry} valueChangedHandler={(value) => this.props.changeHandler(value, 'weaponry')} />
					</div>
				</div>
				<div className="social">
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
						<FiveDots className="dots animalKen" value={this.props.skills.animalKen} valueChangedHandler={(value) => this.props.changeHandler(value, 'animalKen')} />
						<FiveDots className="dots empathy" value={this.props.skills.empathy} valueChangedHandler={(value) => this.props.changeHandler(value, 'empathy')} />
						<FiveDots className="dots expression" value={this.props.skills.expression} valueChangedHandler={(value) => this.props.changeHandler(value, 'expression')} />
						<FiveDots className="dots intimidation" value={this.props.skills.intimidation} valueChangedHandler={(value) => this.props.changeHandler(value, 'intimidation')} />
						<FiveDots className="dots persuasion" value={this.props.skills.persuasion} valueChangedHandler={(value) => this.props.changeHandler(value, 'persuasion')} />
						<FiveDots className="dots socialize" value={this.props.skills.socialize} valueChangedHandler={(value) => this.props.changeHandler(value, 'socialize')} />
						<FiveDots className="dots streetwise" value={this.props.skills.streetwise} valueChangedHandler={(value) => this.props.changeHandler(value, 'streetwise')} />
						<FiveDots className="dots subterfuge" value={this.props.skills.subterfuge} valueChangedHandler={(value) => this.props.changeHandler(value, 'subterfuge')} />
					</div>
				</div>
			</div>
		)
	}
}
