import React from 'react'
import FiveDots from '../FiveDots'
import './Skills.css'

export default class Skills extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			mental: {
				academics: 0,
				computer: 0,
				crafts: 0,
				investigation: 0,
				medicine: 0,
				occult: 0,
				politics: 0,
				science: 0
			},
			physical: {
				athletics: 0,
				brawl: 0,
				drive: 0,
				firearms: 0,
				larceny: 0,
				stealth: 0,
				survival: 0,
				weaponry: 0
			},
			social: {
				animalKen: 0,
				empathy: 0,
				expression: 0,
				intimidation: 0,
				socialize: 0,
				streetwise: 0,
				subterfuge: 0
			}
		}
	}
	
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
						<FiveDots className="dots academics" value={this.state.mental.academics} />
						<FiveDots className="dots computer" value={this.state.mental.computer} />
						<FiveDots className="dots crafts" value={this.state.mental.crafts} />
						<FiveDots className="dots investigation" value={this.state.mental.investigation} />
						<FiveDots className="dots medicine" value={this.state.mental.medicine} />
						<FiveDots className="dots occult" value={this.state.mental.occult} />
						<FiveDots className="dots politics" value={this.state.mental.politics} />
						<FiveDots className="dots science" value={this.state.mental.science} />
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
						<FiveDots className="dots athletics" value={this.state.physical.athletics} />
						<FiveDots className="dots brawl" value={this.state.physical.brawl} />
						<FiveDots className="dots drive" value={this.state.physical.drive} />
						<FiveDots className="dots firearms" value={this.state.physical.firearms} />
						<FiveDots className="dots larceny" value={this.state.physical.larceny} />
						<FiveDots className="dots stealth" value={this.state.physical.stealth} />
						<FiveDots className="dots survival" value={this.state.physical.survival} />
						<FiveDots className="dots weaponry" value={this.state.physical.weaponry} />
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
						<FiveDots className="dots animalKen" value={this.state.social.animalKen} />
						<FiveDots className="dots empathy" value={this.state.social.empathy} />
						<FiveDots className="dots expression" value={this.state.social.expression} />
						<FiveDots className="dots intimidation" value={this.state.social.intimidation} />
						<FiveDots className="dots persuasion" value={this.state.social.persuasion} />
						<FiveDots className="dots socialize" value={this.state.social.socialize} />
						<FiveDots className="dots streetwise" value={this.state.social.streetwise} />
						<FiveDots className="dots subterfuge" value={this.state.social.subterfuge} />
					</div>
				</div>
			</div>
		)
	}
}
