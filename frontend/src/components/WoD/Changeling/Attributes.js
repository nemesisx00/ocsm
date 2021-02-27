import React from 'react'
import FiveDots from '../FiveDots'
import './Attributes.css'

export default class Attributes extends React.Component
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
					<FiveDots className="dots intelligence" value={this.state.power.intelligence} />
					<FiveDots className="dots wits" value={this.state.finesse.wits} />
					<FiveDots className="dots resolve" value={this.state.resistance.resolve} />
				</div>
				<div className="column">
					<div className="label">Strength:</div>
					<div className="label">Dexterity:</div>
					<div className="label">Stamina:</div>
				</div>
				<div className="column">
					<FiveDots className="dots strength" value={this.state.power.strength} />
					<FiveDots className="dots dexterity" value={this.state.finesse.dexterity} />
					<FiveDots className="dots stamina" value={this.state.resistance.stamina} />
				</div>
				<div className="column">
					<div className="label">Presence:</div>
					<div className="label">Manipulation:</div>
					<div className="label">Composure:</div>
				</div>
				<div className="column">
					<FiveDots className="dots presence" value={this.state.power.presence} />
					<FiveDots className="dots manipulation" value={this.state.finesse.manipulation} />
					<FiveDots className="dots composure" value={this.state.resistance.composure} />
				</div>
			</div>
		)
	}
}
