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
					<div className="label">Strength:</div>
					<div className="label">Dexterity:</div>
					<div className="label">Stamina:</div>
				</div>
				<div className="column">
					<FiveDots className="dots strength" value={this.props.attributes.strength} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'strength')} />
					<FiveDots className="dots dexterity" value={this.props.attributes.dexterity} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'dexterity')} />
					<FiveDots className="dots stamina" value={this.props.attributes.stamina} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'stamina')} />
				</div>
				<div className="column">
					<div className="label">Charisma:</div>
					<div className="label">Manipulation:</div>
					<div className="label">Composure:</div>
				</div>
				<div className="column">
					<FiveDots className="dots charisma" value={this.props.attributes.charisma} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'charisma')} />
					<FiveDots className="dots manipulation" value={this.props.attributes.manipulation} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'manipulation')} />
					<FiveDots className="dots composure" value={this.props.attributes.composure} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'composure')} />
				</div>
				<div className="column">
					<div className="label">Intelligence:</div>
					<div className="label">Wits:</div>
					<div className="label">Resolve:</div>
				</div>
				<div className="column">
					<FiveDots className="dots intelligence" value={this.props.attributes.intelligence} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'intelligence')} />
					<FiveDots className="dots wits" value={this.props.attributes.wits} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'wits')} />
					<FiveDots className="dots resolve" value={this.props.attributes.resolve} valueChangedHandler={(arg) => this.props.changeHandler(arg, 'resolve')} />
				</div>
			</div>
		)
	}
}
