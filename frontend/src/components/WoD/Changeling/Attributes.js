import './Attributes.css'
import React from 'react'
import FiveDots from '../FiveDots'

export default class Attributes extends React.Component
{
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
					<FiveDots className="dots intelligence" value={this.props.attributes.power.intelligence} />
					<FiveDots className="dots wits" value={this.props.attributes.finesse.wits} />
					<FiveDots className="dots resolve" value={this.props.attributes.resistance.resolve} />
				</div>
				<div className="column">
					<div className="label">Strength:</div>
					<div className="label">Dexterity:</div>
					<div className="label">Stamina:</div>
				</div>
				<div className="column">
					<FiveDots className="dots strength" value={this.props.attributes.power.strength} />
					<FiveDots className="dots dexterity" value={this.props.attributes.finesse.dexterity} />
					<FiveDots className="dots stamina" value={this.props.attributes.resistance.stamina} />
				</div>
				<div className="column">
					<div className="label">Presence:</div>
					<div className="label">Manipulation:</div>
					<div className="label">Composure:</div>
				</div>
				<div className="column">
					<FiveDots className="dots presence" value={this.props.attributes.power.presence} />
					<FiveDots className="dots manipulation" value={this.props.attributes.finesse.manipulation} />
					<FiveDots className="dots composure" value={this.props.attributes.resistance.composure} />
				</div>
			</div>
		)
	}
}
