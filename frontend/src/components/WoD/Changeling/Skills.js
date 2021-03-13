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
						<FiveDots className="dots academics" value={this.props.skills.mental.academics} />
						<FiveDots className="dots computer" value={this.props.skills.mental.computer} />
						<FiveDots className="dots crafts" value={this.props.skills.mental.crafts} />
						<FiveDots className="dots investigation" value={this.props.skills.mental.investigation} />
						<FiveDots className="dots medicine" value={this.props.skills.mental.medicine} />
						<FiveDots className="dots occult" value={this.props.skills.mental.occult} />
						<FiveDots className="dots politics" value={this.props.skills.mental.politics} />
						<FiveDots className="dots science" value={this.props.skills.mental.science} />
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
						<FiveDots className="dots athletics" value={this.props.skills.physical.athletics} />
						<FiveDots className="dots brawl" value={this.props.skills.physical.brawl} />
						<FiveDots className="dots drive" value={this.props.skills.physical.drive} />
						<FiveDots className="dots firearms" value={this.props.skills.physical.firearms} />
						<FiveDots className="dots larceny" value={this.props.skills.physical.larceny} />
						<FiveDots className="dots stealth" value={this.props.skills.physical.stealth} />
						<FiveDots className="dots survival" value={this.props.skills.physical.survival} />
						<FiveDots className="dots weaponry" value={this.props.skills.physical.weaponry} />
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
						<FiveDots className="dots animalKen" value={this.props.skills.social.animalKen} />
						<FiveDots className="dots empathy" value={this.props.skills.social.empathy} />
						<FiveDots className="dots expression" value={this.props.skills.social.expression} />
						<FiveDots className="dots intimidation" value={this.props.skills.social.intimidation} />
						<FiveDots className="dots persuasion" value={this.props.skills.social.persuasion} />
						<FiveDots className="dots socialize" value={this.props.skills.social.socialize} />
						<FiveDots className="dots streetwise" value={this.props.skills.social.streetwise} />
						<FiveDots className="dots subterfuge" value={this.props.skills.social.subterfuge} />
					</div>
				</div>
			</div>
		)
	}
}
