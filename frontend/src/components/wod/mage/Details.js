import '../Details.css'
import React from 'react'

export default class Details extends React.Component
{
	render()
	{
		return (
			<div className="details">
				<div className="row">
					<label>Shadow Name:</label>
					<input type="text" value={this.props.details.shadowName} onChange={(e) => this.props.changeHandler({shadowName: e.target.value})} />
				</div>
				<div className="row">
					<label>Player:</label>
					<input type="text" value={this.props.details.player} onChange={(e) => this.props.changeHandler({player: e.target.value})} />
				</div>
				<div className="row">
					<label>Chronicle:</label>
					<input type="text" value={this.props.details.chronicle} onChange={(e) => this.props.changeHandler({chronicle: e.target.value})} />
				</div>
				<div className="row">
					<label>Virtue:</label>
					<input type="text" value={this.props.details.virtue} onChange={(e) => this.props.changeHandler({virtue: e.target.value})} />
				</div>
				<div className="row">
					<label>Vice:</label>
					<input type="text" value={this.props.details.vice} onChange={(e) => this.props.changeHandler({vice: e.target.value})} />
				</div>
				<div className="row">
					<label>Concept:</label>
					<input type="text" value={this.props.details.concept} onChange={(e) => this.props.changeHandler({concept: e.target.value})} />
				</div>
				<div className="row">
					<label>Path:</label>
					<input type="text" value={this.props.details.path} onChange={(e) => this.props.changeHandler({path: e.target.value})} />
				</div>
				<div className="row">
					<label>Order:</label>
					<input type="text" value={this.props.details.order} onChange={(e) => this.props.changeHandler({order: e.target.value})} />
				</div>
				<div className="row">
					<label>Legacy:</label>
					<input type="text" value={this.props.details.legacy} onChange={(e) => this.props.changeHandler({legacy: e.target.value})} />
				</div>
			</div>
		)
	}
}
