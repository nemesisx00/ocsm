import '../Attributes.css'
import React from 'react'
import Tracker from '../Tracker'

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
					<Tracker keyWord="strength" className="extended dots strength" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.strength} changeHandler={(arg) => this.props.changeHandler(arg, 'strength')} />
					<Tracker keyWord="dexterity" className="extended dots dexterity" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.dexterity} changeHandler={(arg) => this.props.changeHandler(arg, 'dexterity')} />
					<Tracker keyWord="stamina" className="extended dots stamina" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.stamina} changeHandler={(arg) => this.props.changeHandler(arg, 'stamina')} />
				</div>
				<div className="column">
					<div className="label">Charisma:</div>
					<div className="label">Manipulation:</div>
					<div className="label">Composure:</div>
				</div>
				<div className="column">
					<Tracker keyWord="charisma" className="extended dots charisma" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.charisma} changeHandler={(arg) => this.props.changeHandler(arg, 'charisma')} />
					<Tracker keyWord="manipulation" className="extended dots manipulation" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.manipulation} changeHandler={(arg) => this.props.changeHandler(arg, 'manipulation')} />
					<Tracker keyWord="composure" className="extended dots composure" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.composure} changeHandler={(arg) => this.props.changeHandler(arg, 'composure')} />
				</div>
				<div className="column">
					<div className="label">Intelligence:</div>
					<div className="label">Wits:</div>
					<div className="label">Resolve:</div>
				</div>
				<div className="column">
					<Tracker keyWord="intelligence" className="extended dots intelligence" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.intelligence} changeHandler={(arg) => this.props.changeHandler(arg, 'intelligence')} />
					<Tracker keyWord="wits" className="extended dots wits" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.wits} changeHandler={(arg) => this.props.changeHandler(arg, 'wits')} />
					<Tracker keyWord="resolve" className="extended dots resolve" type={Tracker.Types.Circle} max={this.props.max} value={this.props.attributes.resolve} changeHandler={(arg) => this.props.changeHandler(arg, 'resolve')} />
				</div>
			</div>
		)
	}
}
