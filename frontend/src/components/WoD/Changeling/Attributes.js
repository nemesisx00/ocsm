import React from 'react'
import CheckDot from '../CheckDot'
import './Attributes.css'

class Attributes extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			power: {
				intelligence: props.attributes.power.intelligence,
				strength: props.attributes.power.strength,
				presence: props.attributes.power.presence
			},
			finesse: {
				wits: props.attributes.finesse.wits,
				dexterity: props.attributes.finesse.dexterity,
				manipulation: props.attributes.finesse.manipulation
			},
			resistance: {
				stamina: props.attributes.resistance.stamina,
				composure: props.attributes.resistance.composure,
				resolve: props.attributes.resistance.resolve
			}
		}
	}
	
	render()
	{
		return (
			<div className="attributes">
				<div className="column">
					<div className="label">Intelligence:</div>
					<div className="label">Wits:</div>
					<div className="label">Resolve:</div>
				</div>
				<div className="column">
					<div className="dots intelligence">
						<CheckDot checked={this.state.power.intelligence > 0} />
						<CheckDot checked={this.state.power.intelligence > 1} />
						<CheckDot checked={this.state.power.intelligence > 2} />
						<CheckDot checked={this.state.power.intelligence > 3} />
						<CheckDot checked={this.state.power.intelligence > 4} />
					</div>
					<div className="dots wits">
						<CheckDot checked={this.state.finesse.wits > 0} />
						<CheckDot checked={this.state.finesse.wits > 1} />
						<CheckDot checked={this.state.finesse.wits > 2} />
						<CheckDot checked={this.state.finesse.wits > 3} />
						<CheckDot checked={this.state.finesse.wits > 4} />
					</div>
					<div className="dots resolve">
						<CheckDot checked={this.state.resistance.resolve > 0} />
						<CheckDot checked={this.state.resistance.resolve > 1} />
						<CheckDot checked={this.state.resistance.resolve > 2} />
						<CheckDot checked={this.state.resistance.resolve > 3} />
						<CheckDot checked={this.state.resistance.resolve > 4} />
					</div>
				</div>
				<div className="column">
					<div className="label">Strength:</div>
					<div className="label">Dexterity:</div>
					<div className="label">Stamina:</div>
				</div>
				<div className="column">
					<div className="dots strength">
						<CheckDot checked={this.state.power.strength > 0} />
						<CheckDot checked={this.state.power.strength > 1} />
						<CheckDot checked={this.state.power.strength > 2} />
						<CheckDot checked={this.state.power.strength > 3} />
						<CheckDot checked={this.state.power.strength > 4} />
					</div>
					<div className="dots dexterity">
						<CheckDot checked={this.state.finesse.dexterity > 0} />
						<CheckDot checked={this.state.finesse.dexterity > 1} />
						<CheckDot checked={this.state.finesse.dexterity > 2} />
						<CheckDot checked={this.state.finesse.dexterity > 3} />
						<CheckDot checked={this.state.finesse.dexterity > 4} />
					</div>
					<div className="dots stamina">
						<CheckDot checked={this.state.resistance.stamina > 0} />
						<CheckDot checked={this.state.resistance.stamina > 1} />
						<CheckDot checked={this.state.resistance.stamina > 2} />
						<CheckDot checked={this.state.resistance.stamina > 3} />
						<CheckDot checked={this.state.resistance.stamina > 4} />
					</div>
				</div>
				<div className="column">
					<div className="label">Presence:</div>
					<div className="label">Manipulation:</div>
					<div className="label">Composure:</div>
				</div>
				<div className="column">
					<div className="dots presence">
						<CheckDot checked={this.state.power.presence > 0} />
						<CheckDot checked={this.state.power.presence > 1} />
						<CheckDot checked={this.state.power.presence > 2} />
						<CheckDot checked={this.state.power.presence > 3} />
						<CheckDot checked={this.state.power.presence > 4} />
					</div>
					<div className="dots manipulation">
						<CheckDot checked={this.state.finesse.manipulation > 0} />
						<CheckDot checked={this.state.finesse.manipulation > 1} />
						<CheckDot checked={this.state.finesse.manipulation > 2} />
						<CheckDot checked={this.state.finesse.manipulation > 3} />
						<CheckDot checked={this.state.finesse.manipulation > 4} />
					</div>
					<div className="dots composure">
						<CheckDot checked={this.state.resistance.composure > 0} />
						<CheckDot checked={this.state.resistance.composure > 1} />
						<CheckDot checked={this.state.resistance.composure > 2} />
						<CheckDot checked={this.state.resistance.composure > 3} />
						<CheckDot checked={this.state.resistance.composure > 4} />
					</div>
				</div>
			</div>
		)
	}
}

export default Attributes
